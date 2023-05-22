'use strict'
const baseUrl = 'https://localhost:7219';
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

// console.log(firstName);
// console.log(lastName);
// console.log(email);
// console.log(password);
// console.log(confirmPassword);
// console.log(gender);
// console.log(studentStatus);
// console.log(formFile);




let getRequest = async (url) => {
    const fetching = await fetch(`${baseUrl}${url}`);
    const data = fetching.json();
    return data;
};
const genderList = getRequest('/api/Enum/GetGender');
const studentList = getRequest('/api/Enum/StudentshipStatus');

const genderFiller = async (dropDownList1) => {
    try {
        const a = await dropDownList1;
        for (let item in a) {
            var option = document.createElement("option");
            option.value = a[item];
            option.text = item;
            gender.appendChild(option);
        };
    } catch (error) {
        // console.log( 'this is catch error')
        console.error(error)
    }
};
const studentFiller = async (dropDownList) => {
    try {
        const a = await dropDownList;
        for (let item in a) {
            console.log("key: ", item);
            console.log("value: ", a[item]);
            var option = document.createElement("option");
            option.value = a[item];
            option.text = item;
            studentStatus.appendChild(option);
        };
    } catch (error) {
        // console.log( 'this is catch error')
        console.error(error)
    }
};
genderFiller(genderList);
studentFiller(studentList);
// const dropDownFiller = async (dropDownList) => {
//     try {
//         const a = await dropDownList;

//         console.log('help',a);
//         for (var i = 0; i < a.length; i++) {
//             console.log('dsdsd');
//             console.log(a[i]);
//             var option = document.createElement("option");
//             option.value = dropDownList[i].value;
//             option.text = dropDownList[i].name;
//             gender.appendChild(option);
//         }
//     } catch (error) {
//         console.log('Error: ', error)
//     }
// };

// const dropDownFiller = async (dropDownList) => {
//     try {
//         const a = await dropDownList;
//         console.log(a);

//         a.forEach(element => {
//             console.log(element);
//         });
//     } catch (error){
//         console.log('Error: ',error)
//     }
// };
// {"type":"https://tools.ietf.org/html/rfc7231#section-6.5.1","title":"One or more validation errors occurred.","status":400,"traceId":"00-1612e5680cb980b7bbfee31b58885169-111e5a9201e8ceb2-00","errors":{"Email":["The Email field is required."],"LastName":["The Last Name field is required."],"Password":["The Password field is required.","The Password field must be at least 8 characters long and contains uppercase letter, lowercase letter, number and symbol."],"FirstName":["The First Name field is required."],"ConfirmPassword":["The Re-enter Password field is required."]}}
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



submitButton.addEventListener('click',
    async function copyValue(e) {
        e.preventDefault();
        // const ss = await data();
        // console.log('to be sent datad', ss);
        // console.log(ss);
        const dataValues = {
            FirstName: firstName.value,
            LastName: lastName.value,
            Email: email.value,
            Password: password.value,
            ConfirmPassword: confirmPassword.value,
            Gender: gender.value,
            formFile: formFile.value,
            StudentshipStatus: studentStatus.value
        };


        let formData = new FormData();
        formData.append("FirstName", firstName.value);
        formData.append("LastName", lastName.value);
        formData.append("Email", email.value);
        formData.append("Password", password.value);
        formData.append("ConfirmPassword", confirmPassword.value);
        formData.append("Gender", gender.value);
        formData.append("StudentshipStatus", studentStatus.value);

        console.log("inside ",formData);
        console.log('done with copy next gender');
        await postRequest(`${baseUrl}/api/User/UserRegistration`, dataValues);
        // console.log(dropDownFiller(genderList));


    }
);




// console.log('this line');
// console.log(data);
// const newMet = async () => {
//     const v = await statt;
//     console.log(v);
//     return v;
// };
// console.log(newMet());
// // for (let i = 0; i < statt.length; i++) {
// //     // const element = array[i];
// //     console.log(statt[i]);
// //     console.log(statt[i]);

// // }
// console.log('dsd');
// statt.then(function(x){
//     console.log(x.Student);
// });



// const url = 'https://localhost:7219/api/User/UserRegistration';
async function postRequest(url, data) {
    console.log('this is fetch methos', url);
    console.log("fetxg data",data);
    const fetBody = {
        headers: {
            "content-Type": "application/json"
        },
        method: "POST",
        body: JSON.stringify(data)
    };

    // let fetching = await fetch('https://localhost:7219/api/User/UserRegistration',
    const fetching = await fetch('https://localhost:7219/api/User/UserRegistration', fetBody);
    // {
    console.log('ghjhjhjh', JSON.stringify(formData));
    //     method: "POST",
    //     headers: {
    //         'Content-Type': 'application/json'
    //     },
    //     body: JSON.stringify(data)
    // });

    let postResponse = fetching.json();
    return postResponse;
};
