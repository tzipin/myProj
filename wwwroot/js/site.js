const uri = '/book';
let list = [];

getToken = () => {
    return sessionStorage.getItem("token");
};

function getItems() {
    const token = getToken();
    if(!token)
        window.location.href = "/html/login.html";
    fetch(uri, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'token': token,
        },
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            console.log(response);            
            return response.json();
        })
        .then(data => {
            console.log(data);
            _displayItems(data);
        })
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const addNameTextbox = document.getElementById('add-name');

    const item = {
        id: 0,
        BookName: addNameTextbox.value.trim(),
        isOnlyAdults: false,
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${getToken()}`,
        },
        body: JSON.stringify(item),
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(() => {
            getItems();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${getToken()}`,
        },
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = list.find(item => item.id === id);

    document.getElementById('edit-name').value = item.BookName;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isOnlyAdults').checked = item.isOnlyAdults;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isOnlyAdults: document.getElementById('edit-isOnlyAdults').checked,
        BookName: document.getElementById('edit-name').value.trim(),
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${getToken()}`,
        },
        body: JSON.stringify(item),
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = itemCount === 1 ? 'book' : 'book kinds';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('list');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isOnlyAdultsCheckbox = document.createElement('input');
        isOnlyAdultsCheckbox.type = 'checkbox';
        isOnlyAdultsCheckbox.disabled = true;
        isOnlyAdultsCheckbox.checked = item.isOnlyAdults;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isOnlyAdultsCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.BookName);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    list = data;
}

// Load items on page load
document.addEventListener('DOMContentLoaded', () => {
    getItems();
});