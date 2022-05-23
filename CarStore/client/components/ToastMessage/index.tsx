import { Snackbar, Alert } from "@mui/material";

const ToastMessage = (props: any) => {
  const { setToast, toast } = props;
  const { open, severity, message } = toast;

  const handleClose = () => {
    setToast({
      ...toast,
      open: false     
    })
  };

  return (
    <Snackbar open={open} autoHideDuration={3000} anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }} onClose={handleClose}>
      <Alert severity={severity} sx={{ width: '100%' }}>
        {message}
      </Alert>
    </Snackbar>
  );
};

export default ToastMessage;