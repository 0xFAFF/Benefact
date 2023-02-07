resource "aws_vpc" "main" {
  tags                 = { "Name" : "${var.prefix}-vpc" }
  cidr_block           = var.vpc_cider
  instance_tenancy     = "default"
  enable_dns_support   = true
  enable_dns_hostnames = true
}

resource "aws_internet_gateway" "main" {
  tags       = { "Name" : "${var.prefix}-gw" }
  vpc_id     = aws_vpc.main.id
  depends_on = [aws_vpc.main]
}

resource "aws_subnet" "subnet_a" {
  tags       = { "Name" : "${var.prefix}-subnet-a" }
  vpc_id     = aws_vpc.main.id
  cidr_block = var.subnet_a
}

resource "aws_subnet" "subnet_b" {
  tags       = { "Name" : "${var.prefix}-subnet-b" }
  vpc_id     = aws_vpc.main.id
  cidr_block = var.subnet_b
}

resource "aws_route_table" "public" {
  tags   = { "Name" : "${var.prefix}-route-table" }
  vpc_id = aws_vpc.main.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.main.id
  }
}

resource "aws_route_table_association" "subnet_a" {
  subnet_id      = aws_subnet.subnet_a.id
  route_table_id = aws_route_table.public.id
}

resource "aws_route_table_association" "subnet_b" {
  subnet_id      = aws_subnet.subnet_b.id
  route_table_id = aws_route_table.public.id
}

resource "aws_security_group" "task_sg" {
  vpc_id = aws_vpc.main.id
  ingress {
    from_port       = 80
    to_port         = 80
    protocol        = "tcp"
    prefix_list_ids = ["pl-3b927c52"] # TODO: Lookup from map instead (this is us-east-1)
  }
}

resource "aws_security_group" "instance" {
  vpc_id = aws_vpc.main.id
  ingress {
    from_port       = 22
    to_port         = 22
    protocol        = "tcp"
    security_groups = ["sg-0bcbf003d1932630b"] # TODO: Lookup VPN sg id
  }
}

resource "aws_lb_listener" "front_end" {
  load_balancer_arn = aws_lb.main.arn
  port              = "80"
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.main.arn
  }
}

resource "aws_lb_target_group" "main" {
  name     = "${var.prefix}-tg"
  port     = 80
  protocol = "HTTP"
  vpc_id   = aws_vpc.main.id
}

resource "aws_lb" "main" {
  name               = "${var.prefix}-lb"
  load_balancer_type = "application"
  ip_address_type    = "ipv4"
  depends_on         = [aws_internet_gateway.main]
  security_groups    = [aws_security_group.task_sg.id]
  subnets            = [aws_subnet.subnet_a.id, aws_subnet.subnet_b.id]
}
