'use strict'
const baseUrl = "https://localhost:7219/api/";
const postRequest = async (url, data) => {
    //this is the second parameter to be recieved by fetch
    //fetchBodyObj = {method:'',headers:'',body:JSON.stringify(dataObj)}
    const fetchBody = {
        method: "POST",
        headers: {
            "content-Type": "application/json"
        },
        body: JSON.stringify(data)
    }
    //calling the fetchin method
    // const fetching = await fetch ('url','fetchBodyObj');
    const fetching = await fetch(`${baseUrl}${url}`, fetchBody);
    //convert the promise to json string
    let postResponse = fetching.json();
    return postResponse;
};

// const getRequest = async () =>{}; basic function or mmethod
const getRequest = async (url) =>{
    const fetching = await fetch(`${baseUrl}${url}`);
    let getResponse =  fetching.json();
    return getResponse;
};