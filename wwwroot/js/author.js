const url = '/author'

const getAuthor = () => {
    fetch(url,{
        method: 'GET',
        headers: {
            'Authorizathion': `Bearer ${getToken()}`,
        },
    })
    .then(response => {
        if(!response.ok)
            console.log(response.status);
        else
             return response.json();           
    })
    .then(data => {
        displayAuthor(data);
    })
}

const displayAuthor = (data) => {
    const table = document.getElementById('authorTable');
    table.style.display = 'block';
    const tBody = document.getElementById('author');
    tBody.innerHTML = '';

    // _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        console.log(item.Name);
        let textNode = document.createTextNode(item.name);
        td1.appendChild(textNode);
        

        let td2 = tr.insertCell(1);
        let email = document.createTextNode(item.email)
        td2.appendChild(email);

        let td3 = tr.insertCell(2);
        let address = document.createTextNode(item.address)
        td3.appendChild(address);

        let td4 = tr.insertCell(3);
        let level = document.createTextNode(item.level)
        td4.appendChild(level);

        let td5 = tr.insertCell(4);
        td5.appendChild(editButton);

        let td6 = tr.insertCell(4);
        td6.appendChild(deleteButton);
    });
    // window.location.href = "/html/author.html";    
}