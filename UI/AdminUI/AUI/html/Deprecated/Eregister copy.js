'use strict'
const baseUrl = 'https://localhost:7219';
var firstName = document.getElementById("inputFirstName");
var lastName = document.getElementById("inputLastName");
var email = document.getElementById("inputEmail");
var password = document.getElementById("inputPassword");
var confirmPassword = document.getElementById("inputConfirmPassword");
var gender = document.getElementById("genderDropDown");
var formFile = document.getElementById("uploadFormFile");
var studentStatus = document.getElementById("studentshipStatusDropDown");
var submitButton = document.getElementById("submitBTN");

// console.log(firstName);
// console.log(lastName);
// console.log(email);
// console.log(password);
// console.log(confirmPassword);
// console.log(gender);
// console.log(studentStatus);
// console.log(formFile);

var getRequest = async (url) => {
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
        console.error( error)
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
        console.error( error)
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

const data = () => {
    let dataValues = {
        firstName: firstName.value,
        lastName: lastName.value,
        email: email.value,
        password: password.value,
        confirmPassword: confirmPassword.value,
        gender: gender.value,
        formFile: formFile.value,
        studentStatus: studentStatus.value,
    };
    return dataValues;
};



submitButton.addEventListener('click',
    async function copyValue() {
        console.log(data());
        console.log('done with copy next gender');
        // await genderFiller(genderList);
        // await studentFiller(studentList);
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
const postRequest = async (url, data) => {

    const fetching = await fetch(`${url}`,
        {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

    let postResponse = fetching.json();
    return postResponse;
};