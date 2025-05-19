 submitLogin = async (event) => {
    event.preventDefault();
    const name = document.getElementById("name").value;
    const password = document.getElementById("password").value;

    const author = {
        Id: 0,
        Name: encodeURIComponent(name),
        Password: encodeURIComponent(password),
        Email: "",
        Address: "",
        Level: 0,
    };

    try {
        const response = await fetch('/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(author),
        });
        console.log(response.status);
        
        if (response.ok) {
            const result = await response.text();
            const token = result.split(" ")[0];
            const type = result.split(" ")[1];
            const id = result.split(" ")[2];
            sessionStorage.setItem("token", token);
            sessionStorage.setItem("type", type);
            sessionStorage.setItem("id", id);
            window.location.href = "/";
        } else {
            const error = await response.text();
            //throw new Error(error);
            console.log(error);            
        }
    } catch (error) {
        console.error("Error during login:", error);
        alert("An error occurred. Please try again.");
    }
}