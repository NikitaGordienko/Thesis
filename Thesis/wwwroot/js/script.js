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


    $('.map .pin').click(function(){
        //$(this).find('.object-info').toggleClass('hidden');
    });


    $('.object-info-events-link').click(function(){
        $('.object-events').toggleClass('hidden');
    });


    $('.object-events-add').click(function(){
        $('.create-event-wrap-table').addClass('active');
        $('header, main, footer').addClass('blurry');
    });

    $('.create-event-form-close').click(function(){
        $('.create-event-wrap-table').removeClass('active');
        $('header, main, footer').removeClass('blurry');
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
});