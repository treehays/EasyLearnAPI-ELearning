'use strict'
const baseUrl = 'https://localhost:7219/api/';
const email = document.querySelector("#inputEmail");
const password = document.querySelector("#inputPassword");
const submitBTN = document.querySelector("#submitBTN");

console.log(email);
console.log(password);

let bodyData = () => {
    const data = {
        email: email.value,
        password: password.value
    }
    return data;
};

// let allLocalClaim = () => {
//     const data = {

//     };
//     return data;
// };

submitBTN.addEventListener('click',
    async function copyValue(e) {
        e.preventDefault();
        let responses = await postRequest('User/Login', bodyData());
        console.log("This is all", responses);
        // console.log("This is ad", responses.data);
        if (responses.success) {
            console.log("jwt", responses.data.jwToken)
            localStorage.setItem('token', responses.data.jwToken);
            localStorage.setItem("firstname", responses.data.firstName);
            localStorage.setItem("id", responses.data.id);
            localStorage.setItem("profilePicture", responses.data.profilePicture);
            localStorage.setItem("roleId", responses.data.roleId);
            localStorage.setItem("userName", responses.data.userName);
            // let a = "ds";
            // let b = a.toString().length;
            // console.log("help me check", responses.data.roleId.length === 5);
            if (responses.data.roleId.length !== 0) {
                location.href = `/AUI/html/${responses.data.roleId}Dashboard.html`;
            }
        } else {
            location.href = 'home.html';
        }
    }
);

const postRequest = async (url, data) => {
    let fetchRequst = {
        method: 'POST',
        headers: {
            'content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }
    try {
        const fetching = await fetch(`${baseUrl}${url}`, fetchRequst);
        let postResponse = fetching.json();
        return postResponse;
    } catch (error) {
        console.log(error);
        return undefined;
    }
};