$(document).ready(function () {
    // Real-time validation cho email
    $('#Email').on('input', function () {
        var emailPattern = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;
        if (!emailPattern.test($(this).val())) {
            $(this).next('.text-danger').text("Invalid email format.");
        } else {
            $(this).next('.text-danger').text("");
        }
    });

    // Real-time validation cho password
    $('#Password').on('input', function () {
        var passwordPattern = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^A-Za-z0-9])[A-Za-z\d\W]{6,}$/;
        if (!passwordPattern.test($(this).val())) {
            $(this).next('.text-danger').text("Password must be at least 6 characters, include at least 1 uppercase letter, 1 lowercase letter, 1 digit, and 1 special character.");
        } else {
            $(this).next('.text-danger').text("");
        }
    });
});
