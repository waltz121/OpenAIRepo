
document.getElementById("batch-Submit").disabled = true;

var connection = new signalR.HubConnectionBuilder().withUrl("/batchuploadhub").build();

connection.start().then(function () {
    document.getElementById("batch-Submit").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("UpdateProgressBar", function (progress) {
    var bar = new ldBar("#progressbar-circle");
    bar.set(progress);
});

document.getElementById("batch-Submit").addEventListener("click", function (event) {
    var sqlStatement = document.getElementById("sqlStatement").value;
    // Disable batch submit button
    document.getElementById("batch-Submit").disabled = true;
    connection.invoke("SubmitSqlStatement", sqlStatement).then(function () {
        document.getElementById("batch-Submit").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });
});