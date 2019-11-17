var _chatWindow = null;
var _messages = [];
var connection = null;

//Event Handlers
function AddFriend_Click() {
    $.post("/Friends/Add", { Username: $("#AddFriendUsername").val() }, function (result) {
        if (result !== "")
            alert(result);
    });
}

function FriendListItem_Clicked(object) {
    $(object).addClass("selected");
    OpenFriendChat($(object).attr("data-id"));
}

function ChatPanel_Scrolled(sender) {
    if (sender.scrollTop === 0) {
        LoadPreviousMessages();
    }
}

//User functions
async function OpenFriendChat(friendID) {
    _chatWindow.CurrentFriendID = friendID;
    await GetMessagesForFriendID(friendID, new Date(), true, false);

}

async function LoadPreviousMessages() {
    var currentID = Number(_chatWindow._currentFriendID);
    var currentChatMessages = _messages.filter(msg => msg.recipientID === currentID || msg.senderID === currentID);
    currentChatMessages.sort(function (a, b) { if (a < b) { return 1; } if (a == b) { return 0; } else { return -1; } });

    //var oldestMessageID = $("#ChatPanel .messageLine").first().data("id");
    var oldestMessageTS = currentChatMessages[0].timestamp;
    await GetMessagesForFriendID(currentID, oldestMessageTS, false, true);
}


function GetMessagesForFriendID(ID, BeforeTime, Scroll, Prepend) {
    return new Promise(resolve => {
        var data = { FriendID: ID, Before: BeforeTime };
        $.getJSON("/Messages/GetFriendMessagesBeforeTime", data, function (result) {
            var ids = _messages.map(msg => msg.id);

            result.forEach(function (msg) {
                if (ids.includes(msg.id) == false)
                    _messages.push(msg);
            });

            _chatWindow.Render(Scroll, Prepend);
            resolve();
        });
    });
}

function UpdateFriendList() {
    $.getJSON("/Friends/GetFriends").done(function (result) {
        var friendList = $("#FriendList");
        var template = $("#FriendRowTemplate");

        //Clear existing friend list entries
        friendList.children().not("#FriendRowTemplate").remove();

        result.forEach(function (current) {
            var newRow = template.clone();
            newRow.removeAttr("id").removeAttr("style").text(current.username).attr("data-id", current.id);
            friendList.append(newRow);
        });
    });
}

//ChatHub events
function SendMessage(Message) {
    connection.invoke("SendMessage", Message.RecipientID, Message.Contents);
}

function ReceiveMessage(msg) {
    if (msg.id != _chatWindow.CurrentFriendID)
        $(".friendListItem[data-id=" + msg.id + "]").css("font-weight", "bold");

    _messages.push(msg);

    _chatWindow.Render(true, false);
    _lastMessageCheckTime = new Date().toUTCString();
}

$(document).ready(function () {
    _chatWindow = new ChatWindow($("#ChatWindow"));
    _chatWindow.SendMessageEvent = SendMessage;

    var jobs = [
        { func: UpdateFriendList, interval: 60000 }
    ];

    jobs.forEach(function (job) {
        job.func();

        setInterval(job.func, job.interval);
    });

    connection = new signalR.HubConnectionBuilder().withUrl("chathub").build();
    connection["ServerTimeout"] = 60;
    connection["KeepAliveInterval"] = 30;
    connection.on("KeepAlive", function () { console.log("ChatHub KeepAlive") });
    connection.on("ReceiveMessage", ReceiveMessage);
    connection.on("ReceiveUserStatus", function (result) {

    });
    connection.start();
});