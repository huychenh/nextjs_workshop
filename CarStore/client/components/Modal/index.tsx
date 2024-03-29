import React from "react";
import Button from "@mui/material/Button";
import { styled } from "@mui/material/styles";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogActions from "@mui/material/DialogActions";
import IconButton from "@mui/material/IconButton";
import CloseIcon from "@mui/icons-material/Close";
import { ModalProps } from "./Model";
const BootstrapDialog = styled(Dialog)(({ theme }) => ({
  "& .MuiDialogContent-root": {
    padding: theme.spacing(2),
  },
  "& .MuiDialogActions-root": {
    padding: theme.spacing(1),
  },
}));

export interface DialogTitleProps {
  id: string;
  children?: React.ReactNode;
  onClose: () => void;
}

const BootstrapDialogTitle = (props: DialogTitleProps) => {
  const { children, onClose, ...other } = props;

  return (
    <DialogTitle sx={{ m: 0, p: 2 }} {...other}>
      {children}
      {onClose ? (
        <IconButton
          aria-label="close"
          onClick={onClose}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <CloseIcon />
        </IconButton>
      ) : null}
    </DialogTitle>
  );
};

const Modal = (props: ModalProps) => {
  const { flag, title, children, handleOnOff, handleClear, handleSearch } =
    props;
  return (
    <div>
      <BootstrapDialog
        onClose={handleOnOff}
        aria-labelledby="customized-dialog-title"
        open={flag}
      >
        <BootstrapDialogTitle
          id="customized-dialog-title"
        >
          <div style={{ textAlign: "center" }}>
            <b>{title}</b>
          </div>
        </BootstrapDialogTitle>
        {children}
        <DialogActions>
          <Button autoFocus onClick={handleClear}>
            Clear
          </Button>
          <Button autoFocus onClick={handleSearch}>
            Search
          </Button>
        </DialogActions>
      </BootstrapDialog>
    </div>
  );
};
export default Modal;
