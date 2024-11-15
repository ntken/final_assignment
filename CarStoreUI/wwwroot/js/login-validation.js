$(document).ready(function () {
    // Chỉ validate email real-time (nếu cần)
    $('#Email').on('input', function () {
        var emailPattern = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;
        if (!emailPattern.test($(this).val())) {
            $(this).next('.text-danger').text("Invalid email format.");
        } else {
            $(this).next('.text-danger').text("");
        }
    });
});
