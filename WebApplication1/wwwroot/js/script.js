function go(loc, item) {
    const items = document.getElementById('headNav').children;
    for (const element of items) {
        element.style.fontSize = '20px';
    }
    if (item == 'acount') {
        loc = '/Pages/Login.html'
    }
    document.getElementById(item).style.fontSize = '25px';
    console.log(document.getElementById(item).style.fontSize);
    document.getElementById('main').src = loc;
}