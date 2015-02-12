﻿$(document).ready(function () {
    $(document).on('click', '.showRating', function () {
        $(this).nextAll(".rating-panel").find('.rating-star').removeClass('filled selected');
        $(this).nextAll(".rating-panel").toggle();
    })

    $(document).on('mouseenter', ".rating-star", function () {
        $(this).prevUntil(".rating-area").addClass('filled');
        $(this).addClass('filled');
    })
    $(document).on('mouseleave', ".rating-star", function () {
        if ($(this).parent().find(".rating-star").hasClass('selected')) {
            var selected = $(this).parent().find('.selected').first();
            selected.nextUntil('input :submit').removeClass('filled')
        }
        else {
            $(this).prevUntil(".rating-area").removeClass('filled');
            $(this).removeClass('filled');
        }
    })
    $(document).on('click', ".rating-star", function () {
        $(this).parent().find(".rating-star").removeClass('filled selected');
        $(this).prevUntil(".rating-area").addClass('filled');
        $(this).addClass('filled selected');
        var radioChecked = $(this).next();
        radioChecked.attr('checked', true);
        var $selectedStar = $(this).parent().find(".selected").first().next();
        var $rating = $selectedStar.val();
        $(this).parent().find('#rating-value').val($rating);
    })
    $(document).on('click', '.btn-clear-rating', function () {
        $(this).parent().find('.rating-star').removeClass('filled selected');
    })



    //$(document).on('click', '#btn-add-comment', function () {
    //    $('#last-comment').show();
    //    $('#last-comment').bind('DOMSubtreeModified', function () {
    //        $(this).removeAttr('id');
    //        $(this).addClass('comment');
    //        addEmptyComment();
    //    })

    //})

    $(document).on('click', '#show-hide-comments', function () {
        var comments = $('.panel-comments-content').first();
        comments.toggle();
        if (comments.is(":visible")) {
            $(this).html('Hide comments')
        }
        else {
            $(this).html('Show comments')
        }
    })
})

function addNewComment(result) {
    $('#comment-error>ul>li').hide();
    $('#last-comment').show();
    $('#last-comment').addClass('comment');
    $('#last-comment').removeAttr('id');
    addEmptyComment();
}

function showCommentSanitizeError(data) {
    $('#comment-error>ul>li').hide();
    $('<h4 class="text-danger">' + data.responseJSON.errorMessage + '</h4>')
        .insertBefore('#comment-error')
        .delay(3500)
        .fadeOut();
}