window.onload = function() {
    var elements = document.querySelectorAll("button");
    const doSth = function() {
        [].map.call(elements, function(elem) { elem.classList.remove("active") });
        this.classList.add("active");
    };
    [].map.call(elements, function(elem) {
        elem.addEventListener("click", function() {
            const id = this.parentElement.id;
            showPassword(id);
        });
    });
}

async function showPassword(id) {
    const password = document.getElementById(`secretPassword${id}`);
    const btn = document.getElementById(`showPasswordBtn${id}`);
    if (password.type === "password") {
        sendRequest(id);
    } else {
        password.type = "password";
        password.textContent = "FakePassword";
        btn.textContent = "Show password";
    }
}

async function sendRequest(id) {
    const params = new URLSearchParams({
        masterPassword: `${document.getElementById(`masterPassword${id}`).value}`,
        serviceName: `${document.getElementById(`serviceName${id}`).textContent}`,
        userName: `${document.getElementById("UserName").value}`
    });
    const response = await fetch(`${url}/Secrets/Password?${params.toString()}`,
        {
            method: "GET",
            headers: {
                'Accept': "application/json",
                'Content-Type': "application/json",
                'Authorization': `Bearer ${document.getElementById("Token").value}`
            }
        }).catch(function(error) {
        console.log(error);
    });

    if (!response.ok) {
        alert("Invalid password");
        return;
    }

    const data = await response.json();

    await displayPassword(data, id);
}

async function displayPassword(data, id) {
    const password = document.getElementById(`secretPassword${id}`);
    const btn = document.getElementById(`showPasswordBtn${id}`);
    password.type = "text";
    password.value = data;
    btn.textContent = "Hide password";
}