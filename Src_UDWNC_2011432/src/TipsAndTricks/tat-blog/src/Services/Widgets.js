// import axios from 'axios';
// export async function getCategories() {
// try {
//  const response = await 
// axios.get('https://localhost:7142/api/categories');
//  const data = response.data;
//  if (data.isSuccess)
//  return data.result;
//  else
//  return null;
//  } catch (error) {
//  console.log('Error', error.message);
//  return null;
//  }
// }
import axios from "axios";

export async function getCategories(){

    const axiosClient = axios.create({
        baseURL: 'https://localhost:7142/api/',
        headers: {
          'Content-Type': 'application/json',
        },
      });
      
    const params = {
        PageSize : 10,
        PageNumber: 1
    }
    try{
        const response = await
        axiosClient.get('categories', {params})

        const data = response.data
        if (data.isSuccess)
            return data.result
        else
            return null
    }catch(error){
        console.log('Error',error.message);
     return null
    }
}

