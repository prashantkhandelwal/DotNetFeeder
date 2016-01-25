(function () {
    'use strict';

    var elem_feed_heading = $('.heading');
    var elem_bookmark = $('.bookmark');
    var elem_starIcon = $('.bookmark span');
    var elem_storage = $('#storage');
    var storage = $('#storage option:selected').text();
    var elem_mongolab = $('#mongolab');

    function showStorageSettings(storageName) {
        switch (storageName) {
            case 'Mongolab':
                elem_mongolab.css('display', 'inline');
                break;
            default:
                elem_mongolab.css('display', 'none');
        }
    }

    function bookmarkFeed(bookmark) {
        $.ajax({
            type: "POST",
            url: "/bookmarks",
            data: bookmark,
            datatype: "html",
            success: function (data) {
                toastr.success('Bookmark Saved');
            }
        });
    }

    elem_feed_heading.mouseover(function (e) {
        $(this)[0].childNodes[1].childNodes[3].style.display = 'inline';
    });
    elem_feed_heading.mouseleave(function (e) {
        $(this)[0].childNodes[1].childNodes[3].style.display = 'none';
    });

    elem_starIcon.click(function (e) {
        bookmarkFeed($(this).attr('data-feed'));
    });

    elem_bookmark.css('display', 'none');
    elem_mongolab.css('display', 'none');

    elem_storage.change(function () {
        var selectedStorage = $(this).val();
        showStorageSettings(selectedStorage);
    });

    showStorageSettings(storage);

})();