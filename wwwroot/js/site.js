const uri = '/book';
let list = [];
// const librarian = document.getElementsByClassName('librarian');
// if(getType() === 'Librarian'){
//     for (let i = 0; i < librarian.length; i++) {
//         librarian[i].style.display = 'block';
//     }
// }

getToken = () => {
    return sessionStorage.getItem("token");
}

getType = () => {
    return sessionStorage.getItem("type");
}

getId = () => {
    return sessionStorage.getItem("id");
}

function getItems() {
    const token = getToken();
    if(!token)
        window.location.href = "/html/login.html";
    fetch(uri, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
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
    const addIsOnlyAdults = document.getElementById('add-isOnlyAdults');
    const authorId = document.getElementById('add-author').value;
    const item = {
        id: 0,
        BookName: addNameTextbox.value.trim(),
        isOnlyAdults: addIsOnlyAdults.checked,
        authorId: parseInt(authorId),
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


    
const addAuthor = document.getElementById('add-author');

const selected = () => {
    addAuthor.innerHTML = '';
    authorList.forEach(item => {
        mySelectedAuthor(item);
    });
    const addAuthorId = document.getElementById('add-author');
    return parseInt(addAuthorId.value);
}

const mySelectedAuthor = (item) => {
    let option = document.createElement('option');
    option.value = item.id;
    option.text = item.name;
    addAuthor.appendChild(option);
}

// getAuthorsForSelect();
// selected();

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
    console.log(list);
    
    const item = list.find(item => item.id === id);

    document.getElementById('edit-authorId').value = item.authorId;
    document.getElementById('edit-name').value = item.BookName;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isOnlyAdults').checked = item.isOnlyAdults;
    document.getElementById('editForm').style.display = 'block';
}

function displayAuthorEditForm(id) {
    console.log(authorList);
    
    const item = authorList.find(item => item.id === id);
    console.log(item);
    document.getElementById('edit-AuthorId').value = item.id;
    document.getElementById('edit-AuthorPassword').value = item.password;
    document.getElementById('edit-AuthorName').value = item.name;
    document.getElementById('edit-AuthorAdrress').value = item.address;
    document.getElementById('edit-AuthorEmail').value = item.email;
    const authorLevel = document.getElementById('edit-AuthorLevel');
    if(getType() === 'Librarian'){
        let option = document.createElement('option');
        option.value = "1";
        option.text = "Librarian";
        authorLevel.appendChild(option);
    }
    document.getElementById('editAuthorForm').style.display = 'block';
}

const updateAuthor = () => {
    const authorId = document.getElementById('edit-AuthorId').value;
    const authorPassword = document.getElementById('edit-AuthorPassword').value;
    const authorName = document.getElementById('edit-AuthorName').value;
    const authorAddress = document.getElementById('edit-AuthorAdrress').value;
    const authorEmail = document.getElementById('edit-AuthorEmail').value;
    const authorLevel = document.getElementById('edit-AuthorLevel').value;

    const item = {
        id: parseInt(authorId),
        password: authorPassword,
        name: authorName,
        address: authorAddress,
        email: authorEmail,
        level: parseInt(authorLevel),
    };

    fetch(`${url}/${authorId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${getToken()}`,
        },
        body: JSON.stringify(item),
    })
        .then(() => getAuthor())
        .catch(error => console.error('Unable to update item.', error));

    closeFormEditAuthor();
}

const closeFormEditAuthor = () => {
    document.getElementById('editAuthorForm').style.display = 'none';
}


function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const authorId = document.getElementById('edit-authorId').value;
    const item = {
        id: parseInt(itemId, 10),
        isOnlyAdults: document.getElementById('edit-isOnlyAdults').checked,
        BookName: document.getElementById('edit-name').value.trim(),
        authorId: parseInt(authorId),
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

const closeeditAuthorForm = () => {
    document.getElementById('editAuthorForm').style.display = 'none';
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
        console.log(item.bookName);
        let textNode = document.createTextNode(item.bookName);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(2);
        td4.appendChild(deleteButton);
    });

    list = data;
}

// Load items on page load
document.addEventListener('DOMContentLoaded', () => {
    getItems();
});