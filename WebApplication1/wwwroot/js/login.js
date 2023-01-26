
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
        let res = response.text();
        if (response.status === 200) {
            window.parent.login(res);
        }
        return res;
    }).then((res) => {
        console.log(res);
        window.parent.document.getElementById('failed').innerHTML = res;
    }).catch((error) => {
        console.error(error);
    });
}