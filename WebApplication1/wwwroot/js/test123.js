
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
        window.parent.login(response.json());
    }).catch((error) => {
        window.parent.document.getElementById('failed').innerHTML = error;
        console.error(error);
    });
}