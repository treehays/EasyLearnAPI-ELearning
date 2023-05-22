'use strict'
const baseUrl = "https://localhost:7219/api/";
const postRequest = async (url, data) => {
    const fetchBody = {
        method: "POST",
        headers: {
            "content-Type": "application/json"
        },
        body: JSON.stringify(data)
    }
    const fetching = await fetch(`${baseUrl}${url}`, fetchBody);
    let postResponse = fetching.json();
    return postResponse;
};

const getRequest = async (url) =>{
    const fetching = await fetch(`${baseUrl}${url}`);
    let getResponse =  fetching.json();
    return getResponse;
};