const defaultOptions = {
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    mode: 'cors',
    method: 'GET'
  };
  
export default function fetchJson(url: string, options: any) {
    const reqOptions = {...defaultOptions, ...options};

    return fetch(url, reqOptions);
}