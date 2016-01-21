(function () {
    'use strict';

    var elem_feed_heading = $('.heading');
    var elem_bookmark = $('.bookmark');

    elem_feed_heading.mouseover(function (e) {
        $(this)[0].childNodes[1].childNodes[3].style.display = 'inline';
    });
    elem_feed_heading.mouseleave(function (e) {
        $(this)[0].childNodes[1].childNodes[3].style.display = 'none';
    });

    elem_bookmark.css('display', 'none');

})();