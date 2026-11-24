$(function () {
    // Show or hide password fields
    $(".toggle-password").on("click", function () {
        const target = $($(this).data("target"));
        const icon = $(this).find("i");
        const isHidden = target.attr("type") === "password";

        target.attr("type", isHidden ? "text" : "password");
        icon.toggleClass("bi-eye bi-eye-slash");
    });

    // Hide success messages after 4 seconds
    setTimeout(function () {
        $(".auto-dismiss").fadeOut(400);
    }, 4000);
});