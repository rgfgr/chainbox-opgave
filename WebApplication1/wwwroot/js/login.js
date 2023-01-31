﻿let failtext = window.parent.document.getElementById('failed');
function login() {
    const uri = 'api/Members';
    const email = document.getElementById('email').value;
    const psw = document.getElementById('psw').value;
    console.log(uri);
    console.log(email);
    console.log(psw);
    let pass = JSON.stringify(psw);
    console.log(pass);
    fetch(uri + '/Login/' + document.getElementById('email').value, {
        method: 'post',
        body: pass,
        headers: {
            'Accept': 'text/plain',
            'Content-Type': 'application/json'
        }
    }).then((response) => {
        console.log(response);
        failtext.innerHTML = '';
        window.parent.login(response.text());
    }).catch((error) => {
        failtext.innerHTML = error;
        console.error(error);
    });
}