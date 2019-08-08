function validateLoginForm() {
    console.log("---------in validate");
    if (document.getElementById("txtUsername").value == "") {
        alert("Please Enter User Name");
        document.getElementById("txtUsername").focus();
        return false;

    }
    if (document.getElementById("txtPassword").value == "") {
        alert("Please Enter Password");
        document.getElementById("txtPassword").focus();
        return false;
    }
    MessPassoword();
}

function MessPassoword() {
    console.log("---------mess");
    console.log(document.getElementById("txtUsername").value);
    console.log(document.getElementById("txtPassword").value);
    var web_url = document.getElementById("hdnWebUrl").value;
    console.log(web_url);
    var result = GetSynchronousJSONResponse(web_url, '{"Name":"' + document.getElementById('txtUsername').value + '"}');
    result = eval('(' + result + ')');
    digest = Crypto.MD5(document.getElementById("txtPassword").value);
    finaldigest = digest.toUpperCase();
    hash = finaldigest + result.d;
    var finalvalue = Crypto.MD5(hash);
    console.log(document.getElementById("txtPassword").value);
    document.getElementById("txtPassword").value = finalvalue;
}


function GetSynchronousJSONResponse(url, postData) {
    console.log("------------------URL TEST : "+url);
    var xmlhttp = null;
    if (window.XMLHttpRequest) {
        console.log("xmlhttp");
        xmlhttp = new XMLHttpRequest();
    }
    else if (window.ActiveXObject) {
        console.log("ativex");
        if (new ActiveXObject("Microsoft.XMLHTTP")) {
            console.log("axmlhttp");
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        } else {
            console.log("msxxmlhttp");
            xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
        } 
    }

    url = url + "?rnd=" + Math.random(); // to be ensure non-cached version

    xmlhttp.open("POST", url, false);
    xmlhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xmlhttp.send(postData);
    var responseText = xmlhttp.responseText;
    console.log(responseText);
    return responseText;
}