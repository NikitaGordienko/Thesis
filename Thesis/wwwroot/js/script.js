$(document).ready(function(){

    $('.filter-item-option-checkbox').click(function () {
        $(this).children('.filter-item-option-check').toggleClass('checked');
        var checkbox = $(this).children('input[type=checkbox]');
        checkbox.prop("checked", !checkbox.prop("checked"));
    });


    $('.header-auth-profile').click(function(){
        $('.header-auth-profile-menu').toggleClass('active');
    });


    $('.auth-remember').click(function(){
        $(this).children('.auth-remember-check').toggleClass('checked');
        var checkbox = $(this).children('input[type=checkbox]');
        checkbox.prop("checked", !checkbox.prop("checked"));
    });


    // превью загруженного аватара
    function previewAvatar(input) {

        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#profile-avatar-preview').attr('src', e.target.result);
            };

            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#upload-avatar-input").change(function () {
        previewAvatar(this);
    });


    // Скрываем пункты, если их больше 4
    $('.filter-item-options').each(function(){
        if ($(this).children('.filter-item-option').length <= 4) {
            $(this).children('.filter-item-showall').remove();
        }
        $(this).children('.filter-item-option').each(function(e){
            if (e > 3) {
                $(this).addClass('hidden');
            }
        });
    });


    $('.filter-item-showall').click(function(){
        var currentItem = $(this).parents('.filter-item-options');
        if(!$(this).hasClass('hide')) {
            currentItem.children('.filter-item-option').each(function(e){
                $(this).removeClass('hidden');
            });
            $(this).text("- Свернуть");
            $(this).addClass('hide');
        }
        else {
            currentItem.children('.filter-item-option').each(function(j){
                if (j > 3) {
                    $(this).addClass('hidden');
                }
            });
            $(this).text("+ Показать");
            $(this).removeClass('hide');
        }
    });


    $('.profile-photo-upload-button').click(function(){
        $(this).parents('.profile-photo-upload').children('input').click();
    });


    $('.search-result-question a').click(function(){
        $('.add-object-wrap-table').addClass('active');
        $('header, main, footer').addClass('blurry');
    });

    $('.add-object-form-close').click(function(){
        $('.add-object-wrap-table').removeClass('active');
        $('header, main, footer').removeClass('blurry');
    });

    /* Select в добавлении площадки */
    $('.add-object-type-dropdown').click(function () {
        $(this).attr('tabindex', 1).focus();
        $(this).toggleClass('active');
        $(this).find('.add-object-type-dropdown-menu').slideToggle(100);
    });
    $('.add-object-type-dropdown').focusout(function () {
        $(this).removeClass('active');
        $(this).find('.add-object-type-dropdown-menu').slideUp(100);
    });
    $('.add-object-type-dropdown .add-object-type-dropdown-menu li').click(function () {
        $(this).parents('.add-object-type-dropdown').find('span').text($(this).text());
        $(this).parents('.add-object-type-dropdown').find('input').attr('value', $(this).attr('id'));
    });
    /* /Select в добавлении площадки */


    /* Radio в добавлении площадки */
    $('.add-object-input-indicator').click(function(){
        $(this).find('span').addClass('active');
        $(this).parents('.add-object-input-light').find('input[type=radio]').prop('checked', true);
        var radioOff = $('.add-object-input-light').find('input[type=radio]').not(':checked');
        radioOff.parents('.add-object-input-light').find('span').removeClass('active');

    });

    /* /Radio в добавлени площадки */



    /* Регистрация */
    $('.field-validation-error').parent('.auth-form-field').find('.auth-form-label').addClass("validation-error");
    /* /Регистрация */


    /* Создание события */

    $('.object-events-add').click(function () {
        var objectId = $(this).parents('.object-events').data('objectid');
        $('#createEvent #ObjectId').val(objectId);

        $('.create-event-wrap-table').addClass('active');
        $('header, main, footer, .object-events').addClass('blurry');
    });


    $('#createEvent #Date').datepicker({
        firstDay: 1,
        dateFormat: 'dd.mm.yy',
        minDate: 0
    });
    
    function DateParse(ee) {
        var dateVal = ee.val();
        var dateDay = dateVal.substr(0, 2);
        var dateMonth = dateVal.substr(3, 2);
        var dateYear = dateVal.substr(6, 4);
        var dateParse = dateMonth + "." + dateDay + "." + dateYear;
        return dateParse;
    }

    $('#TimeFrom, #TimeTo').focusout(function () {
        if (IsValidTime($('#TimeFrom')) == "correct" && IsValidTime($('#TimeTo')) == "correct") {
            CompareInsertedTime();
        }
    });

    // проверка введенного времени на корректность
    function IsValidTime(e) {
        if (e.val() !== "") {
            var hourVal = e.val().substr(0, 2);
            var minVal = e.val().substr(3, 2);
            if (hourVal > 23 || minVal > 59) {
                e.val("");
                return false;
            }
            else {
                return "correct";
            }
        }
    }

    function CompareInsertedTime() {
        var timeFrom = $('#TimeFrom');
        var timeTo = $('#TimeTo');
        var hourValFrom = timeFrom.val().substr(0, 2);
        var minValFrom = timeFrom.val().substr(3, 2);
        var hourValTo = timeTo.val().substr(0, 2);
        var minValTo = timeTo.val().substr(3, 2);

        if (hourValFrom > hourValTo) {
            timeTo.val("");
            return false;
        }
        if (hourValFrom == hourValTo) {
            if (minValFrom > minValTo) {
                timeTo.val("");
                return false;
            }
        }
    }

    $('#createEvent').submit(function (e) {

        //не отправлять форму (чтобы страница не перезагружалась)
        e.preventDefault();

        if ($('#createEvent .input-validation-error').length == 0) {

            var eventDate = DateParse($('#Date'));
            var eventTimeFrom = eventDate + ' ' + $('#TimeFrom').val() + ':00';
            var eventTimeTo = eventDate + ' ' + $('#TimeTo').val() + ':00';
            var model = {
                //ObjectId: $('#createEvent').data('objectid'),
                ObjectId: $('#ObjectId').val(),
                Date: eventDate + ' 0:00:00',
                TimeFrom: eventTimeFrom,
                TimeTo: eventTimeTo,
                Description: $('.create-event-input-descr').val()
            };

            var data = JSON.stringify(model);

            $.ajax(
                {
                    url: "/Map/CreateEvent",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: data,
                    success: function (result) {
                        var htmlToAppend = `
                            <div class="object-events-item">
                                <div class="object-event-top clearfix">
                                    <div class="object-event-creator">
                                        <img src="` + result.avatar + `" alt="">
                                    </div>
                                    <div class="object-event-datetime">
                                        <div class="object-event-date">` + result.date + `</div>
                                        <div class="object-event-time">` + result.timeFrom + ` - ` + result.timeTo + `</div>
                                    </div>
                                </div>
                                <div class="object-event-descr">
                                    ` + result.description + `
                                </div>
                            </div>
                         `;
                        $('.create-event-form-close').click();
                        $('.object-events').not('.hidden').children('.object-events-list').append(htmlToAppend);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr.status);
                        console.log(ajaxOptions);
                        console.log(thrownError);
                    }
                }
            ).done(function (res) {
                console.log(res);
                // TODO: отобразить добавленную информацию в списке событий;
            });
        }

    });

    $('.create-event-form-close').click(function () {
        $('.create-event-wrap-table').removeClass('active');
        $('header, main, footer, .object-events').removeClass('blurry');
        $('#createEvent input, #createEvent textarea').removeClass('input-validation-error');
        $('#createEvent input, #createEvent textarea').not("#createEvent input[type=submit]").val("");
    });
    /* /Создание события */


    /* Подписка и отписка */
    $('.search-result-subscribe').click(function () {
        var objectId = $(this).attr('id').substr($(this).attr('id').indexOf('_') + 1);
        if ($(this).hasClass('subscribe')) {
            $.get("/Filter/Subscribe/" + objectId, function (response) {
                if (response.res == "success") {
                    var curObj = $('#subscribe_' + response.resObject);
                    curObj.removeClass('subscribe');
                    curObj.addClass('unsubscribe');
                    curObj.attr('id', 'unsubscribe_' + response.resObject);
                }
            });
        }
        else {
            $.get("/Filter/Unsubscribe/" + objectId, function (response) {
                if (response.res == "success") {
                    var curObj = $('#unsubscribe_' + response.resObject);
                    curObj.removeClass('unsubscribe');
                    curObj.addClass('subscribe');
                    curObj.attr('id', 'subscribe_' + response.resObject);
                }
            });
        }

    });

    /*/ Подписка и отписка */

    /* Фильтр */
    $('#filter-apply').click(function () {

        // объект, в котором будут храниться массивы всех выбранных параметров
        var optionsChosen;

        // Районы
        var optionsDistrict = $('#filter-district .filter-item-options');
        var optionsDistrictChosen = [];
        optionsDistrict.find('.filter-item-option').each(function () {
            var districtInput = $(this).find('input[type=checkbox]');
            if (districtInput.prop('checked') == true) {
                optionDistrictChosen = {
                    id: districtInput.attr('id')
                };
                optionsDistrictChosen.push(optionDistrictChosen);
            }
        });

        // Типы площадок
        var optionsType = $('#filter-objecttype .filter-item-options');
        var optionsTypeChosen = [];
        optionsType.find('.filter-item-option').each(function () {
            var typeInput = $(this).find('input[type=checkbox]');
            if (typeInput.prop('checked') == true) {
                optionTypeChosen = {
                    id: typeInput.attr('id')
                };
                optionsTypeChosen.push(optionTypeChosen);
            }    
        });

        // Покрытия
        var optionsTerrain = $('#filter-terrain .filter-item-options');
        var optionsTerrainChosen = [];
        optionsTerrain.find('.filter-item-option').each(function () {
            var terrainInput = $(this).find('input[type=checkbox]');
            if (terrainInput.prop('checked') == true) {
                optionTerrainChosen = {
                    id: terrainInput.attr('id')
                };
                optionsTerrainChosen.push(optionTerrainChosen);
            }
        });

        // Освещение
        var optionsLight = $('#filter-light .filter-item-options .filter-item-option');
        optionsLightChosen = "";
        count = 0;
        optionsLight.each(function (e) {
            if ($(this).find('input').prop('checked') == true) {
                count++;
            }
        });
        
        if (count == 1) { // если выбран только один вариант
            if (optionsLight.find('#lighttrue').prop('checked') == true) {
                optionsLightChosen = "light-true";
            }
            else {
                optionsLightChosen = "light-false";
            }
        }
        else { // если не выбрано ни одного варианта, либо выбраны оба
            optionsLightChosen = "light-all";
        }


        // создание объекта, передаваемого в метод Filter
        optionsChosen = {
            districtsChosen: optionsDistrictChosen,
            typesChosen: optionsTypeChosen,
            terrainsChosen: optionsTerrainChosen,
            lightChosen: optionsLightChosen
        };

        // отправка на сервер
        var data = JSON.stringify(optionsChosen);
        $('#filter-form').find('input[name=data]').val(data);
        $('#filter-form').submit();

    });

    /* /Фильтр */


    /* Карта на странице фильтра */
    $('.search-result-item-map').click(function () {
        $('.filter-item-location-table').addClass('active');
        $('header, main, footer').addClass('blurry');
        LoadItemMap($(this).data('itemlocation'));
    });

    $('.filter-item-location-table .filter-item-location-close').click(function () {
        $('.filter-item-location-table').removeClass('active');
        $('header, main, footer').removeClass('blurry');
        $(this).parents('.filter-item-location-table').find('#filter-map').remove();
        g = document.createElement('div');
        g.setAttribute("id", "filter-map");
        $('.filter-item-map-wrap').append(g);
    });

    function LoadItemMap(itemLocation) {

        var separatorIndex = itemLocation.indexOf(',');
        var objLatitude = itemLocation.substr(0, separatorIndex);
        var objLongitude = itemLocation.substr(separatorIndex + 1);

        DG.then(function () {
            map = DG.map('filter-map', {
                center: [objLatitude, objLongitude], // маркер по центру,
                maxBounds: [
                    [55.566355, 37.314646],
                    [55.950471, 37.934649]
                ],
                zoom: 14, // оптимальный зум для отображения всей Москвы при загрузке
                zoomControl: false,
                fullscreenControl: false
            });

            markerIcon = DG.icon({
                iconUrl: '/images/marker.png',
                iconSize: [24, 32],
                iconAnchor: [12, 32]
            });

            marker = DG.marker([objLatitude, objLongitude],
                {
                    icon: markerIcon
                }
            ).addTo(map);
        });
    } 

    /* /Карта на странице фильтра */


});