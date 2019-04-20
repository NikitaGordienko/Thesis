$(document).ready(function () {

    GenerateMap();

    function GenerateMap() {

        var map;
        var marker;
        var popup;

        DG.then(function () {
            map = DG.map('map', {
                center: [55.741216, 37.620921], // центр Москвы,
                maxBounds: [
                    [55.566355, 37.314646],
                    [55.950471, 37.934649]
                ],
                zoom: 11, // оптимальный зум для отображения всей Москвы при загрузке
                zoomControl: false,
                fullscreenControl: false
            });

            var markerIcon = DG.icon({
                iconUrl: '/images/marker.png',
                iconSize: [24, 32],
                iconAnchor: [12, 32]
            });


            // получение информации о координатах площадок
            $.get("/Map/GetMarkers", function (result) {
                //console.log(result);
                for (i = 0; i < result.info.length; i++) {

                    var object = result.info[i];
                    var address = object["item2"]; // так как передается кортеж, ключи по умолчанию именуются как item1, item2...
                    // получение широты и долготы, так как передается строка
                    var separatorIndex = address.indexOf(',');
                    var objLatitude = address.substr(0, separatorIndex);
                    var objLongitude = address.substr(separatorIndex + 1);

                    marker = DG.marker([objLatitude, objLongitude],
                        {
                            icon: markerIcon,
                            objectId: object["item1"],
                            _latitude: objLatitude,
                            _longitude: objLongitude
                        }
                    )
                        .on('click', function () {


                            var objId = this.options.objectId;
                            var lat = this.options._latitude;
                            var lng = this.options._longitude;

                            $.get("/Map/GetObjectInfo/" + objId, function (res) {

                                var light;
                                if (res.light === true) {
                                    light = "Есть";
                                }
                                else {
                                    light = "Нет";
                                }
                                
                                popup = DG.popup()
                                    .setLatLng([lat, lng])
                                    .setContent(`
                                        <div class="object-info-wrap">
                                            <div class="object-info" data-objectid="` + objId + `">
                                                <div class="object-info-top">
                                                    <div class="object-info-photo">
                                                        <img src="` + res.photo + `" alt="">
                                                    </div>
                                                    <div class="object-info-type-wrap">
                                                        <div class="object-info-type">` + res.type + `</div>
                                                        <div class="object-info-district">` + res.district + `</div>
                                                    </div>
                                                </div>
                                                <div class="object-info-props">
                                                    <div class="object-info-terrain">Покрытие: <span>` + res.terrain + `</span></div>
                                                    <div class="object-info-light">Освещение: <span>` + light + `</span></div>
                                                </div>
                                                <div class="object-info-events-link-wrap">
                                                    <div class="object-info-events-link" data-clickcount="0">События на спортивном объекте</div>
                                                </div>
                                            </div>
                                        </div>
                                    `); // содержимое попапа

                                popup.openOn(map);
                                var clickCount = $('.object-info-events-link').data('clickcount');

                                $('.object-info-events-link').click(function () {

                                    // Для исправления множественной подгрузки событий
                                    clickCount++;

                                    var objEventsElem = $('.object-events');
                                    objEventsElem.data('objectid', objId);
                                    objEventsElem.removeClass('hidden');

                                    if (clickCount < 2) {
                                        $.get("/Map/GetObjectEvents/" + objId, function (response) {
                                            var eventsHtml = "";
                                            if (response.objEvents.length > 0) {

                                                eventsHtml = `
                                                    <div class="object-events-header">Ближайшие события на площадке</div>
                                                    <div class="object-events-list">
                                               `;
                                                for (i = 0; i < response.objEvents.length; i++) {

                                                    var date;
                                                    var responseDate = new Date(Date.parse(response.objEvents[i].date));
                                                    var responseDateDay = getResponseDateDay(responseDate);
                                                    var responseDateMonth = getResponseDateMonth(responseDate);
                                                    var responseDateYear = responseDate.getFullYear();
                                                    date = responseDateDay + "." + responseDateMonth + "." + responseDateYear;

                                                    eventsHtml += `
                                                        <div class="object-events-item">
                                                            <div class="object-event-top clearfix">
                                                                <div class="object-event-creator">
                                                                    <img src="` + response.objEvents[i].user.avatar.path + `" alt="">
                                                                </div>
                                                                <div class="object-event-datetime">
                                                                    <div class="object-event-date">` + date + `</div>
                                                                    <div class="object-event-time">` + getResponseTime(response.objEvents[i].timeStart) + ` - ` + getResponseTime(response.objEvents[i].timeEnd) + `</div>
                                                                </div>
                                                            </div>
                                                            <div class="object-event-descr">
                                                                ` + response.objEvents[i].description + `
                                                            </div>
                                                        </div> 
                                                    `;
                                                }
                                                eventsHtml += `</div>`;
                                            }
                                            else {
                                                eventsHtml = '<div class="object-events-header">Ближайших событий на площадке нет</div>';
                                            }

                                            $('.object-events').prepend(eventsHtml);

                                        });
                                    }

                                });



                            });


                        })
                        .addTo(map); //.bindPopup("Тест попап")

                }

            });

        });

    }

    // TODO: переписать функции с датами

    function getResponseDateDay(rDate) {
        if (rDate.getDate() < 10) {
            return '0' + rDate.getDate();
        }
        else {
            return rDate.getDate();
        }
    }

    function getResponseDateMonth(rDate) {
        if (rDate.getMonth() + 1 < 10) {
            return '0' + parseInt(rDate.getMonth() + 1);
        }
        else {
            return parseInt(rDate.getMonth() + 1);
        }
    }

    function getResponseTime(rDateTime) {
        var parsedDate = new Date(Date.parse(rDateTime));
        var hour;
        var min;
        if (parsedDate.getHours() < 10) {
            hour = '0' + parsedDate.getHours();
        }
        else {
            hour = parsedDate.getHours();
        }

        if (parsedDate.getMinutes() < 10) {
            min = '0' + parsedDate.getMinutes();
        }
        else {
            min = parsedDate.getMinutes();
        }
        return hour + ":" + min;
    }

    $('#map').click(function () {
        $('.object-events').addClass('hidden');
        $('.object-events').find('.object-events-header').remove();
        $('.object-events').find('.object-events-list').remove();
    });

});


//$(document).on("click", ".hoh", function () { alert(1337) });
