const uri = 'api/Members';
let tBody;

function getMedlemer() {
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
        fetch(uri + '/' + item.id + '/Images')
            .then(response => response.json())
            .then(data => _displayItem(data, item.name))
            .catch(error => console.error('Unable to get images for ' + item.name, error));
    })
}

function _displayItem(data, itemName) {
    console.log(data);
    console.log(itemName);
    let divGal = document.createElement('div');
    divGal.className = 'gallery';

    let image = data[getRndInteger(data.length)];
    let path = '../imgs/' + image.name;

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

function getRndInteger(max) {
    return Math.floor(Math.random() * max);
}