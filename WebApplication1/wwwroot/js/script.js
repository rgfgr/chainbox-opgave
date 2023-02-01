let member;
let logedin = false;

function go(loc, item) {
    console.log('go');
    const items = document.getElementById('headNav').children;
    for (const element of items) {
        element.style.fontSize = '20px';
    }
    document.getElementById(item).style.fontSize = '25px';
    console.log(document.getElementById(item).style.fontSize);
    if (item == 'acount' && !logedin) {
        loc = '/Login.html';
    }
    document.getElementById('main').src = loc;
}

function login(data) {
    member = data;
    console.log(data);
    document.getElementById('failed').innerHTML = '';
    console.log('login');
    logedin = true;
    document.getElementById('logout').style.display = 'block';
    go('/Medlem.html', 'acount');
}

function getMemberId() {
    console.log(member);
    return member;
}

function logout() {
    member = "";
    logedin = false;
    document.getElementById('logout').style.display = 'none';
    console.log(document.getElementById('main').attributes.src.value);
    if (document.getElementById('main').attributes.src.value == '/Medlem.html') {
        go('/Medlem.html', 'acount');
    }
}