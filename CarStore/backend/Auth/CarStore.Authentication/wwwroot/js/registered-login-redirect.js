window.addEventListener("load", function () {
    var a = document.querySelector("a.registered-login-redirect");
    if (a) {

        window.setTimeout(function () {
            window.location.href = a.href;
        }, 5000);
    }
});
