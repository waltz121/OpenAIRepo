// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var $messages = $(".messages-content"),
    d,
    h,
    m,
    i = 0;

$(window).load(function () {
    $messages.mCustomScrollbar();
    setTimeout(function () {
        InitialMessage();
    }, 100);
});

function updateScrollbar() {
    $messages.mCustomScrollbar("update").mCustomScrollbar("scrollTo", "bottom", {
        scrollInertia: 10,
        timeout: 0
    });
}

function setDate() {
    d = new Date();
    if (m != d.getMinutes()) {
        m = d.getMinutes();
        $('<div class="timestamp">' + d.getHours() + ":" + m + "</div>").appendTo(
            $(".message:last")
        );
    }
}

function insertMessage() {
    msg = $(".message-input").val();
    if ($.trim(msg) == "") {
        return false;
    }

    $('<div class="message message-personal">' + msg + "</div>")
        .appendTo($(".mCSB_container"))
        .addClass("new");

    $(".message-input").val(null);
    updateScrollbar();

    chatbotMessage();

}

$(".message-submit").click(function () {
    insertMessage();
});

$(window).on("keydown", function (e) {
    if (e.which == 13) {
        insertMessage();
        return false;
    }
});

async function chatbotMessage() {
    if ($(".message-input").val() != "") {
        return false;
    }
    $(
        '<div class="message loading new"><figure class="avatar"><img src="images/customer-service.svg" /></figure><span></span></div>'
    ).appendTo($(".mCSB_container"));
    updateScrollbar();
    var response = await GetAiResponse();
    $(".message.loading").remove();
    $(
        '<div class="message new"><figure class="avatar"><img src="images/customer-service.svg" /></figure>' +
        SetMessageDisplay(response) +
        "</div>"
    )
        .appendTo($(".mCSB_container"))
        .addClass("new");

    updateScrollbar();
}
async function InitialMessage() {
    if ($(".message-input").val() != "") {
        return false;
    }
    $(
        '<div class="message loading new"><figure class="avatar"><img src="images/customer-service.svg" /></figure><span></span></div>'
    ).appendTo($(".mCSB_container"));
    updateScrollbar();
    var message = await GetInitialResponse();
    $(".message.loading").remove();
    $(
        '<div class="message new"><figure class="avatar"><img src="images/customer-service.svg" /></figure>' +
        SetMessageDisplay(message) +
        "</div>"
    )
        .appendTo($(".mCSB_container"))
        .addClass("new");

    updateScrollbar();
    i++;
}

async function GetAiResponse() {
    var clientMessage = $(".message-input").val();
    var currentContent = [];

    var Messages = $("div.message.new");

    for (var i = 0; i < Messages.length; i++) {
        if (Messages[i].innerText) {
            if (Messages[i].className == "message new") {
                currentContent.push({ role: "assistant", content: Messages[i].innerText })
            } else {
                currentContent.push({ role: "user", content: Messages[i].innerText })
            }
        }
    }

    var dataToSend = { Messages: currentContent, TxtMessage: clientMessage }
    return await $.ajax({
        type: "POST",
        url: "/Home/SendMessage",
        contentType: "application/json",
        data: JSON.stringify(dataToSend)
    })
}

async function GetInitialResponse() {
    return await $.ajax({
        type: "GET",
        url: "/Home/GetInitialChatViewModel",
        contentType: "application/json",
    })
}

function SetMessageDisplay(response) {
    let responseObj = JSON.parse(response.message);
    var stringHtml = "<p class=\"ai-message\">" + responseObj.Message + "</p>";
    var Sources = [];

    //Code for checking undefined value
    if (responseObj.SourceUrl != undefined) {
        Sources = responseObj.SourceUrl;
    }

    if (Sources.length != 0) {
        stringHtml = stringHtml + "<div><label id=\"learn-more\">Learn more: </label></br>";
        for (var i = 0; i < Sources.length; i++) {
            var url = new URL(Sources[i].url);
            var path = url.pathname;
            var sourceExplanation = Sources[i].sourceExplanation;
            stringHtml += (i + 1) + ". <a class=\"article-links\" href='" + url + "' target='_blank'>" + path + "</a> ";
            stringHtml += "<p class=\"source-explanation\">" + sourceExplanation +"</p>";
        }
        stringHtml = stringHtml + "</div>";
    }

    return stringHtml;

}
