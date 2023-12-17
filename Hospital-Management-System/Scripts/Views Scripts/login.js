$(document).ready(function () {
    $('#resetBtn').click(function () {
        $('input[type="text"], input[type="password"]').val('');
        $('.field-validation-error').empty();
    });
});