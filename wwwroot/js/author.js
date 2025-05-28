const url = '/author'
let authorList = [];

if(getType() == "Librarian")
{
    const joining = document.getElementById('joiningAuthorForm');
    joining.style.display = 'block';
}

const Authors = () => {
    console.log(getType());    
     if(getType() === 'Author'){
        console.log(`${url}/${getId()}`);        
        fetch(`${url}/${getId()}`,{
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
            console.log(data);            
            authorList = [data];   
            mySelectedAuthor(data);         
        })
    }
    else{
        fetch(url, {
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
            console.log(data);
            authorList = data;
            selected();
        })
    }
}
Authors();
const getAuthor = () => {
    Authors();
    displayAuthor(authorList);
}

const displayAuthor = (data) => {
    const tBody = document.getElementById('author');
    tBody.innerHTML = '';
    data.forEach(item => {
        myAuthor(item);
    });
    authorList = data;
}
    
    const table = document.getElementById('authorTable');
    const tBody = document.getElementById('author');
    tBody.innerHTML = '';
    const button = document.createElement('button');

    const myAuthor = (item) => {
        table.style.display = 'block';

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayAuthorEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteAuthor(${item.id})`);

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
    }

    const deleteAuthor = (id) => {
        fetch(`${url}/${id}`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${getToken()}`,
            },
        })
            .then(() => getItems())
            .then(() => getAuthor())
            .catch(error => console.error('Unable to delete author.', error));
    }

    const  addAuthorToService = async() => {
        const newAuthor = {
            id: 0,
            name: document.getElementById('add-authorName').value,
            email: document.getElementById('add-authorEmail').value,
            address: document.getElementById('add-authorAddress').value,
            password: document.getElementById('add-authorPassword').value,
            level: document.getElementById('add-authorLevel').value
        };       
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${getToken()}`,
            },
            body: JSON.stringify(newAuthor)
        })
        .then(() => getAuthor())
        .catch(error => console.error('Unable to add author.', error));    
        }   
    
