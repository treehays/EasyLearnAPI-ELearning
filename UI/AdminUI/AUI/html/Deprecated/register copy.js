'use strict'
const baseUrl = 'https://localhost:7219/api/';
const firstName = document.getElementById("inputFirstName");
const lastName = document.getElementById("inputLastName");
const email = document.getElementById("inputEmail");
const password = document.getElementById("inputPassword");
const confirmPassword = document.getElementById("inputConfirmPassword");
const gender = document.getElementById("genderDropDown");
const formFile = document.getElementById("uploadFormFile");
const studentStatus = document.getElementById("studentshipStatusDropDown");
const formSelector = document.getElementById("formData");
var submitButton = document.getElementById("submitBTN");
var notifyText = document.getElementById("ntfTxt");

let getRequest = async (url) => {
    const fetching = await fetch(`${baseUrl}${url}`);
    const data = fetching.json();
    return data;
};
//generating dropdown for gender
const genderFiller = async (dropDownList) => {
    try {
        const a = await dropDownList;
        for (let item in a) {
            var option = document.createElement("option");
            option.value = a[item];
            option.text = item;
            gender.appendChild(option);
        };
    } catch (error) {
        console.error(error)
    }
};
//generating dropdown for student
const studentFiller = async (dropDownList) => {
    try {
        const a = await dropDownList;
        for (let item in a) {
            var option = document.createElement("option");
            option.value = a[item];
            option.text = item;
            studentStatus.appendChild(option);
        };
    } catch (error) {
        console.error(error)
    }
};
//function populating the dropdown list
genderFiller(getRequest('Enum/GetGender'));
studentFiller(getRequest('Enum/StudentshipStatus'));
//data to be sent to the backend
const data = () => {
    let dataValues = {
        FirstName: firstName.value,
        LastName: lastName.value,
        Email: email.value,
        Password: password.value,
        ConfirmPassword: confirmPassword.value,
        Gender: gender.value,
        formFile: formFile.value,
        StudentshipStatus: studentStatus.value
    };
    return dataValues;
};
//the fetching method
async function postRequest(url, data) {
    const fetBody = {
        headers: {
            "content-Type": "application/json"
        },
        method: "POST",
        body: JSON.stringify(data)
    };
    //the fetching method
    const fetching = await fetch(`${baseUrl}${url}`, fetBody);
    let postResponse = fetching.json();
    return postResponse;
};

function handleResponse(responseDd) {
    $(document).ready(function () {
        toastr.options.timeOut = 3500; // 1.5s
        toastr.success(`${responseDd.messages}`);
    });
    notifyText.innerHTML = `<h2> ${responseDd.messages}</h2>`;
}

//Event Copying the vlue from the form
submitButton.addEventListener('click',
    async function copyValue(e) {
        e.preventDefault();
        let responseDd = await postRequest('User/UserRegistration', data());
        if (responseDd.success === true) {
            notifyText.innerHTML = '';
            const msgTxt = document.createElement("h3");
            msgTxt.textContent = responseDd.message;
            console.log("h33cc3", msgTxt.textContent);
            notifyText.appendChild(msgTxt);

            // $(document).ready(function () {
            //     toastr.options.timeOut = 3500; // 1.5s
            //     toastr.success(`${responseDd.messages}`);
            // });
            // notifyText.innerHTML = `<h2> ${responseDd.messages}</h2>`;




            setTimeout(() => {
                window.location.href = "login.html"
            }, 5000);
        } else {
            notifyText.innerHTML = '';
            const msgTxt = document.createElement("h3");
            msgTxt.textContent = responseDd.message;
            console.log("h33cc3", msgTxt.textContent);
            notifyText.appendChild(msgTxt);
        }
    }
);


// //////
// document.getElementById("myForm").addEventListener("submit", async function (event) {
//     event.preventDefault(); // Prevent form submission

//     // Get form data
//     var formData = new FormData(this);

//     try {
//         // Send form data to the server using fetch and wait for the response
//         const response = await fetch("your-endpoint-url", {
//             method: "POST",
//             body: formData
//         });

//         // Parse response as JSON
//         const data = await response.json();

//         // Check the response data and redirect accordingly
//         if (data.success) {
//             window.location.href = "success.html"; // Redirect to success page
//         } else {
//             window.location.href = "error.html"; // Redirect to error page
//         }
//     } catch (error) {
//         console.error("Error:", error);
//         // Handle any error that occurred during the request
//         window.location.href = "error.html"; // Redirect to error page
//     }
// });
