function resetForm() {
    $('input[type="text"], input[type="password"]').val('');
    $('select').prop('selectedIndex', 0);
    $('.field-validation-error').empty();
}

function handleDelete() {
    if (confirm('Are you sure you want to delete this account?')) {
        var data = {
            id: '@ViewBag.item.AccountID',
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        };

        $.ajax({
            url: '@Url.Action("Delete", "Admin")',
            type: 'POST',
            data: data,
            success: function () {
                window.location.href = '@Url.Action("Index", "Admin")';
            }
        });
    }
}

$(document).ready(function () {
    $('#resetBtn').click(function () {
        resetForm();
    });

    $('#deleteBtn').click(function () {
        handleDelete();
    });
});
