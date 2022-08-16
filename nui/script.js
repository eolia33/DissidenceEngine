$(function() {
    window.addEventListener('message', function(event) {
        if (event.data.type == "Open") {
            QBRadio.SlideUp()
        }

        if (event.data.type == "Close") {
            QBRadio.SlideDown()
        }

        if (event.data.type == "Off") {
            document.querySelector('#status').style.backgroundColor = 'red';
            $("#toggle").prop("checked", false);
        }

        if (event.data.type == "On") {
            document.querySelector('#status').style.backgroundColor = 'green';
            $("#toggle").prop("checked", true);
        }
    });

    document.onkeyup = function (data) {
        if (data.which == 27) { // Escape key
            $.post('https://engine/cs:engine:client:tracker:close', JSON.stringify({}));
            QBRadio.SlideDown()
        } else if (data.which == 13) { // Enter key
            $.post('https://engine/cs:engine:client:tracker:close', JSON.stringify({
                channel: $("#channel").val()
            }));
        }
    };
});

document.getElementById('name').onkeypress=function(e){
    if(!(/[a-z]/i.test(String.fromCharCode(e.keyCode)))) {
        e.preventDefault();
        return false;
    }
}
document.getElementById('channel').onkeypress=function(e){
    if(!(/[0-9]/i.test(String.fromCharCode(e.keyCode)))) {
        e.preventDefault();
        return false;
    }
}

QBRadio = {}


$(document).on('click', '#poweredOff', function(e){
    e.preventDefault();

    $.post('https://qb-radio/poweredOff', JSON.stringify({
        channel: $("#channel").val()
    }));
});

QBRadio.SlideUp = function() {
    $(".container").css("display", "block");
    $(".swiper").css("display", "block");
    $(".radio-container").animate({bottom: "6vh",}, 250);
}

QBRadio.SlideDown = function() {
    $(".radio-container").animate({bottom: "-110vh",}, 400, function(){
        $(".swiper").css("display", "none");
        $(".container").css("display", "none");

    });
}

var checkbox = document.getElementById('toggle');
checkbox.addEventListener('change', function() {

    var checkbox = document.getElementById('toggle');
    if(isChecked){
             var checkboxb = document.getElementById('notification');
             var isCheckedb = checkboxb.checked;
             var valueNotification = 0;

             var isCheckedb = checkboxb.checked;

             if(isCheckedb)
             valueNotification = 1;
             else
             valueNotification = 0;


             
             $.post('https://engine/cs:engine:client:tracker:join', JSON.stringify({
                channel: $("#channel").val(),
                name:$("#name").val(),
                notification: valueNotification
            }));
    }
    else
    {
        $.post('https://engine/cs:engine:client:tracker:leave');
        document.querySelector('#status').style.backgroundColor = 'red';
    }
});

var checkboxb = document.getElementById('notification');
checkboxb.addEventListener('change', function() {
    var isCheckedb = checkboxb.checked;
    var checkbox = document.getElementById('toggle');
    var isChecked = checkbox.checked;
    if(isChecked)
    {
         if(isCheckedb)
             $.post('https://engine/cs:engine:client:tracker:notification', JSON.stringify({x:1}));
        else
             $.post('https://engine/cs:engine:client:tracker:notification',JSON.stringify({x:0}));
    }

});

var frequency = document.getElementById("channel")
frequency.oninput = function () {
    if (this.value.length > 4) {
        this.value = this.value.slice(0,4); 
    }
}

var frequency = document.getElementById("name")
frequency.oninput = function () {
    if (this.value.length > 9) {
        this.value = this.value.slice(0,9); 
    }
}
