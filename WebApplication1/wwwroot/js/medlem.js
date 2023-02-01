let tBody;
async function AJAXSubmit(oFormElement) {
    var resultElement = oFormElement.elements.namedItem("result");
    const route = `${oFormElement.action}${window.parent.getMemberId()}&${oFormElement.elements.namedItem("name").value}`;
    const formData = new FormData(oFormElement);
    for (const [key, value] of formData) {
        console.log(`${key}: ${value}`);
    }
    console.log(route);

    try {
        const task = fetch(route, {
            method: 'POST',
            body: formData
        });

        for (var i in formData.entries()) {
            console.log(i);
        }
        const response = await task;
        const res = response.clone();

        if (response.ok) {
            console.log(response.json());
            console.log(res.text());
        }

        resultElement.value = 'Result: ' + response.status + ' ' +
            response.statusText;
    } catch (error) {
        console.error('Error:', error);
    }

    _getImages();
}

async function _getImages() {
    tBody = document.getElementById('img');
    tBody.innerHTML = '';
    const response = await fetch('api/Image/' + window.parent.getMemberId() + '/Images');
    console.log(response);
    const result = await response.json();
    console.log(result);
    for (const image of result) {
        console.log(image);
        _displayItem(image);
    }
}

function _displayItem(image) {
    console.log(image);
    let divGal = document.createElement('div');
    divGal.className = 'gallery';

    let path = '../imgs/' + image.filepath;

    let a = document.createElement('a');
    a.target = '_blank';
    a.href = path;

    let img = document.createElement('img');
    img.src = path;

    let divDesc = document.createElement('div');
    divDesc.className = 'desc';
    divDesc.innerHTML = image.name;

    let btn = document.createElement('button');
    btn.className = 'deletebtn';
    btn.innerHTML = 'delete';
    btn.onclick = event => _deleteImg(image.id);

    divDesc.appendChild(btn);
    a.appendChild(img);
    divGal.appendChild(a);
    divGal.appendChild(divDesc);

    tBody.appendChild(divGal);
}

async function _deleteImg(imageId) {
    const response = await fetch('api/Image/' + imageId, {
        method: 'delete'
    });

    console.log(response);
    _getImages();
}