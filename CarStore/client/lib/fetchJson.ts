const defaultOptions = {
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'Authorization': 'Bear'
    },
    mode: 'cors',
    method: 'GET'
}
  
export default function fetchJson(url: string, accessToken?: string, options?: any) {
    const reqOptions = {...defaultOptions, ...options};

    if (accessToken) {
      reqOptions.headers = { ...reqOptions.headers, 'Authorization': `Bearer ${accessToken}`  };
    }

    return fetch(url, reqOptions);
}