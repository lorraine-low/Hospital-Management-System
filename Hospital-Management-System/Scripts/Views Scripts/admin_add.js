function toggleSpecializationAndBloodGroup() {
    var selectedValue = $('#accountTypeSelect').val();
    if (selectedValue == 'Doctor') {
        $('#specializationRow').show();
        $('#bloodGroupRow').hide();
        // Enable the specialization input and disable the blood group input
        $('#specializationRow input').prop('disabled', false);
        $('#bloodGroupRow select').prop('disabled', true);
    } else if (selectedValue == 'Patient') {
        $('#bloodGroupRow').show();
        $('#specializationRow').hide();
        // Enable the blood group input and disable the specialization input
        $('#bloodGroupRow select').prop('disabled', false);
        $('#specializationRow input').prop('disabled', true);
    } else {
        $('#specializationRow').hide();
        $('#bloodGroupRow').hide();
        // Disable both inputs
        $('#specializationRow input').prop('disabled', true);
        $('#bloodGroupRow select').prop('disabled', true);
    }
}

$(document).ready(function () {
    toggleSpecializationAndBloodGroup();

    $('#resetBtn').click(function () {
        $('input[type="text"], input[type="password"]').val('');
        $('.field-validation-error').empty();
        $('select').prop('selectedIndex', 0);
    });

    $('#accountTypeSelect').change(function () {
        toggleSpecializationAndBloodGroup();
    });
});