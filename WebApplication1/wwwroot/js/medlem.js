
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
}