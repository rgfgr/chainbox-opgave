const uri = 'api/Members';
let tBody;
let member;
let logedin = false;

function getMedlemer() {
    console.log('getMedlemer');
    fetch(uri)
        .then(response => response.json())
        .then(data => getImages(data))
        .catch(error => console.error('Unable to get Members.', error));
}

function getImages(data) {
    console.log(data);
    for (item of data) {
        console.log('test');
        console.log(item);
    }
    tBody = document.getElementById('main');
    tBody.innerHTML = '';
    data.forEach(item => {
        fetch('api/Image/' + item.id + '/Random')
            .then(response => response.json())
            .then(data => _displayItem(data))
            .catch(error => console.error('Unable to get images for ' + item.name, error));
    })
}

function _displayItem(image) {
    let itemName = image.memberName;
    console.log(image);
    console.log(itemName);
    let divGal = document.createElement('div');
    divGal.className = 'gallery';

    let path = '../imgs/' + image.filepath;

    let a = document.createElement('a');
    a.target = '_blank';
    a.href = path;

    let img = document.createElement('img');
    img.src = path;
    img.width = 600;
    img.height = 400;

    let divDesc = document.createElement('div');
    divDesc.className = 'desc';
    divDesc.innerHTML = image.name + ' by ' + itemName;

    a.appendChild(img);
    divGal.appendChild(a);
    divGal.appendChild(divDesc);

    tBody.appendChild(divGal);
}

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