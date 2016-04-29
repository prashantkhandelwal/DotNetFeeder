$(function () {
    var hub = $.connection.feedNotificationHub;
    $.connection.hub.start();
    hub.client.feedJobNotification = function (status) {
        toastr.info(status);
    };
});