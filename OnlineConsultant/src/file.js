import './styles/_main.scss';

/* global $ */

$('.input-validation-error').each(function (idx, el) {
    $(el).after('<span class="glyphicon glyphicon-remove form-control-feedback" aria-hidden="true"></span>')
    $(el).parent().addClass('has-error has-feedback')
})