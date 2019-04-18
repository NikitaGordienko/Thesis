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

                                //console.log(res);                               
                                popup = DG.popup()
                                    .setLatLng([lat, lng])
                                    .setContent(`
                                        <div class="object-info-wrap">
                                            <div class="object-info">
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
                                                    <div class="object-info-light">Освещение: <span>` + res.light + `</span></div>
                                                </div>
                                                <div class="object-info-events-link-wrap">
                                                    <div class="object-info-events-link">События на спортивном объекте</div>
                                                </div>
                                                <div style="font-size: 12px;">` + objId + `</div>
                                            </div>
                                        </div>
                                    `); // содержимое попапа

                                popup.openOn(map);

                            });


                    })
                    .addTo(map); //.bindPopup("Тест попап")
                    
                }

            });

        });

    }




});