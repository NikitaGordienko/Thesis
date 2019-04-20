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


    $('.search-result-question').click(function(){
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


});