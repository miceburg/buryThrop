//When the document loads, run initPage
$(document).ready(initPage);

//declare and instantiate a global variable
var filters = {};

//On page load, get the current list of filters
function initPage()
{
    //GET a JSON object from the server: in this case, the list of filters
    $.ajax({
        type: 'get', //GET or POST
        dataType: 'json', //XML, HTML, script, JSON (type you expect back from server)
        contentType: 'application/json', //send data of this type to the server
        url: 'http://www.cgnoloop.com:8080/Config', //'http://localhost:59552/Config', //Where to send the request
        success: loadContainer //if successful, pass data to this function
    });

    //When the "submit" action is taken on the form "inputPhrase," run addPhrase
    $('#inputPhrase').submit(addPhrase);
}

//Add a new filter to the server
function addPhrase()
{
    //Get the value from the "newFilter" text box
    var newFilter = { "newFilter": $("#newFilter").val() };

    //Check to see if the phrase is already in the list of filters.
    //If it is, cancel the POST and send an alert that the phrase is already included.
    for (var i = 0; i < filters.length; i++)
    {
        if (filters[i] == $("#newFilter").val())
        {
            alert("The phrase \"" + $("#newFilter").val() + "\" is already being filtered. \n Please check your input and resubmit.");
           return false;
        }
    }

    //POST the new filter to the server
    $.ajax({
        type: 'post', //GET or POST
        data: JSON.stringify(newFilter), //data to be sent to the server
        dataType: 'json', //XML, HTML, script, JSON (type you expect back from server)
        contentType: 'application/json', //send data of this type to the server
        url: 'http://www.cgnoloop.com:8080/Config', //'http://localhost:59552/Config', //Where to send the request
        success: loadContainer //if successful, pass data to this function
    });

    //Clear the text box with the user's input
    $("#newFilter").val("");
    
    //Since we're handling the POST manually, cancel any other action
    return false;
}

function loadContainer(data)
{
    //assign the gathered data to a global variable
    filters = data;

    //Sort filters alphabetically
    filters.sort(function (a, b) {
        if (a < b) return -1;
        if (a > b) return 1;
    });

    //create a new string of the filter values
    var refreshList = '';
    for (var i = 0; i < filters.length; i++) {
        refreshList += '<option>' + filters[i] + '</option>';
    }

    //add the string of filters to a list on the HTML page
    $("#currentFilters").html(refreshList);
}

