variable "prefix" {
  type = string
  default = "bugs-dev"
}

variable "vpc_cider" {
  type = string
  default = "10.3.0.0/16"
}

variable "subnet_a" {
  type = string
  default = "10.3.1.0/24"
}

variable "subnet_b" {
  type = string
  default = "10.3.2.0/24"
}