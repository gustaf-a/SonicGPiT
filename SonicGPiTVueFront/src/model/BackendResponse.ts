class BackendResponse<T> {
    data?: T;
    messages?: string[];
    isSuccess: boolean = true;
  }

  export default BackendResponse;