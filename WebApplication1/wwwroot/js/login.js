const uri = 'api/Members';
async function login() {
    const email = document.getElementById('email').value;
    const psw = document.getElementById('psw').value;
    console.log(uri);
    console.log(email);
    console.log(psw);
    let pass = JSON.stringify(psw);
    console.log(pass);
    try {
        const task = fetch(uri + '/Login/' + document.getElementById('email').value, {
            method: 'post',
            body: pass,
            headers: {
                'Accept': 'text/plain',
                'Content-Type': 'application/json'
            }
        })

        const response = await task;
        console.log(response);
        let res = await response.text();
        console.log(res);
        if (response.status === 200) {
            window.parent.login(res);
        }
        else {
            document.getElementById('warn').innerHTML = res;
        };
    } catch (error) {
        console.error('Error: ', error)
    }
}

async function signup() {
    const name = document.getElementById('username').value;
    const email = document.getElementById('suemail').value;
    const pass = document.getElementById('supsw').value;
    const conpsw = document.getElementById('conpsw').value;
    console.log(uri);
    console.log(email);
    console.log(pass);
    if (pass != conpsw) {
        document.getElementById('suwarn').innerHTML = 'Passwords dont match';
        return;
    }
    let member = JSON.stringify({ id: name, email, pass });
    console.log(member);
    try {
        const task = fetch(uri, {
            method: 'post',
            body: member,
            headers: {
                'Accept': 'text/plain',
                'Content-Type': 'application/json'
            }
        })

        const response = await task;
        console.log(response);
        let res = await response.text();
        console.log(res);
        if (response.status === 200) {
            window.parent.login(res);
        }
        else {
            document.getElementById('suwarn').innerHTML = res;
        };
    } catch (error) {
        console.error('Error: ', error)
    }
}