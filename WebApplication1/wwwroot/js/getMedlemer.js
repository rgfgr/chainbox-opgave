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
        console.log(item);
    }
    tBody = document.getElementById('main');
    tBody.innerHTML = '';
    data.forEach(item => {
        fetch(uri + '/' + item.Id + '/Images')
            .then(response => response.json())
            .then(data => _displayItem(data))
            .catch(error => console.error('Unable to get images for ' + item.name, error));
    })
}

function _displayItem(data) {
    console.log(data);
    let divGal = document.createElement('div');
    divGal.className = 'gallery';

    let path = '../imgs/' + data[getRndInteger(data.length)].name;

    let a = document.createElement('a');
    a.target = '_blank';
    a.href = path;

    let img = document.createElement('img');
    img.src = path;
    img.width = 600;
    img.height = 400;

    let divDesc = document.createElement('div');
    divDesc.className = 'desc';

    a.appendChild(img);
    divGal.appendChild(a);
    divGal.appendChild(divDesc);

    tBody.appendChild(divGal);
}

function getRndInteger(max) {
    return Math.floor(Math.random() * max);
}