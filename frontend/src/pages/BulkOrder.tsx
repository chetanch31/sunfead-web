import { useState } from "react";
import Navbar from "@/components/Navbar";
import Footer from "@/components/Footer";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Card } from "@/components/ui/card";
import { Mail, Phone, Package, Truck, Users, CheckCircle } from "lucide-react";

const BulkOrder = () => {
  const [formData, setFormData] = useState({
    name: "",
    company: "",
    email: "",
    phone: "",
    productInterest: "",
    quantity: "",
    message: "",
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log("Bulk order inquiry submitted:", formData);
    alert("Thank you for your bulk order inquiry! Our team will contact you within 24 hours.");
    setFormData({ 
      name: "", 
      company: "", 
      email: "", 
      phone: "", 
      productInterest: "", 
      quantity: "", 
      message: "" 
    });
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  return (
    <div className="min-h-screen bg-background">
      <Navbar />

      {/* Hero Section */}
      <section className="py-16 md:py-24 bg-card border-b">
        <div className="container mx-auto px-4">
          <div className="max-w-3xl mx-auto text-center">
            <h1 className="text-4xl md:text-5xl lg:text-6xl font-display font-bold mb-6">
              Bulk <span className="text-gradient">Orders</span>
            </h1>
            <p className="text-lg md:text-xl text-muted-foreground">
              Perfect for businesses, events, corporate gifting, and large gatherings
            </p>
          </div>
        </div>
      </section>

      {/* Benefits */}
      <section className="py-16 md:py-24">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl md:text-4xl font-display font-bold mb-12 text-center">
            Why Choose Sunfead for Bulk Orders?
          </h2>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 max-w-6xl mx-auto">
            <Card className="p-6 text-center">
              <div className="flex justify-center mb-4">
                <div className="h-14 w-14 rounded-full bg-primary/10 flex items-center justify-center">
                  <Package className="h-7 w-7 text-primary" />
                </div>
              </div>
              <h3 className="font-bold mb-2">Custom Packaging</h3>
              <p className="text-sm text-muted-foreground">
                Branded packaging options available for corporate gifts
              </p>
            </Card>

            <Card className="p-6 text-center">
              <div className="flex justify-center mb-4">
                <div className="h-14 w-14 rounded-full bg-primary/10 flex items-center justify-center">
                  <Truck className="h-7 w-7 text-primary" />
                </div>
              </div>
              <h3 className="font-bold mb-2">Timely Delivery</h3>
              <p className="text-sm text-muted-foreground">
                Guaranteed on-time delivery for your events
              </p>
            </Card>

            <Card className="p-6 text-center">
              <div className="flex justify-center mb-4">
                <div className="h-14 w-14 rounded-full bg-primary/10 flex items-center justify-center">
                  <Users className="h-7 w-7 text-primary" />
                </div>
              </div>
              <h3 className="font-bold mb-2">Dedicated Support</h3>
              <p className="text-sm text-muted-foreground">
                Personal account manager for your orders
              </p>
            </Card>

            <Card className="p-6 text-center">
              <div className="flex justify-center mb-4">
                <div className="h-14 w-14 rounded-full bg-primary/10 flex items-center justify-center">
                  <CheckCircle className="h-7 w-7 text-primary" />
                </div>
              </div>
              <h3 className="font-bold mb-2">Special Pricing</h3>
              <p className="text-sm text-muted-foreground">
                Competitive bulk pricing and volume discounts
              </p>
            </Card>
          </div>
        </div>
      </section>

      {/* Ideal For */}
      <section className="py-16 md:py-24 bg-card">
        <div className="container mx-auto px-4">
          <div className="max-w-4xl mx-auto">
            <h2 className="text-3xl md:text-4xl font-display font-bold mb-8 text-center">
              Ideal For
            </h2>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="flex items-start gap-3">
                <div className="h-6 w-6 rounded-full bg-primary/20 flex items-center justify-center flex-shrink-0 mt-1">
                  <CheckCircle className="h-4 w-4 text-primary" />
                </div>
                <div>
                  <h3 className="font-semibold mb-1">Corporate Events</h3>
                  <p className="text-sm text-muted-foreground">
                    Conferences, seminars, and office celebrations
                  </p>
                </div>
              </div>

              <div className="flex items-start gap-3">
                <div className="h-6 w-6 rounded-full bg-primary/20 flex items-center justify-center flex-shrink-0 mt-1">
                  <CheckCircle className="h-4 w-4 text-primary" />
                </div>
                <div>
                  <h3 className="font-semibold mb-1">Weddings & Functions</h3>
                  <p className="text-sm text-muted-foreground">
                    Traditional celebrations and family gatherings
                  </p>
                </div>
              </div>

              <div className="flex items-start gap-3">
                <div className="h-6 w-6 rounded-full bg-primary/20 flex items-center justify-center flex-shrink-0 mt-1">
                  <CheckCircle className="h-4 w-4 text-primary" />
                </div>
                <div>
                  <h3 className="font-semibold mb-1">Retail Businesses</h3>
                  <p className="text-sm text-muted-foreground">
                    Stock your store with authentic products
                  </p>
                </div>
              </div>

              <div className="flex items-start gap-3">
                <div className="h-6 w-6 rounded-full bg-primary/20 flex items-center justify-center flex-shrink-0 mt-1">
                  <CheckCircle className="h-4 w-4 text-primary" />
                </div>
                <div>
                  <h3 className="font-semibold mb-1">Corporate Gifting</h3>
                  <p className="text-sm text-muted-foreground">
                    Thoughtful gifts for clients and employees
                  </p>
                </div>
              </div>

              <div className="flex items-start gap-3">
                <div className="h-6 w-6 rounded-full bg-primary/20 flex items-center justify-center flex-shrink-0 mt-1">
                  <CheckCircle className="h-4 w-4 text-primary" />
                </div>
                <div>
                  <h3 className="font-semibold mb-1">Restaurants & Cafes</h3>
                  <p className="text-sm text-muted-foreground">
                    Quality ingredients for your menu
                  </p>
                </div>
              </div>

              <div className="flex items-start gap-3">
                <div className="h-6 w-6 rounded-full bg-primary/20 flex items-center justify-center flex-shrink-0 mt-1">
                  <CheckCircle className="h-4 w-4 text-primary" />
                </div>
                <div>
                  <h3 className="font-semibold mb-1">Export Orders</h3>
                  <p className="text-sm text-muted-foreground">
                    International bulk orders welcome
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Contact Form */}
      <section className="py-16 md:py-24">
        <div className="container mx-auto px-4">
          <div className="max-w-5xl mx-auto">
            <div className="text-center mb-12">
              <h2 className="text-3xl md:text-4xl font-display font-bold mb-4">
                Request a Quote
              </h2>
              <p className="text-lg text-muted-foreground">
                Fill out the form below and our team will get back to you with a customized quote
              </p>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-12">
              {/* Contact Information */}
              <div className="space-y-8">
                <div>
                  <h3 className="text-xl font-bold mb-6">Bulk Order Contact</h3>
                  <div className="space-y-4">
                    <div className="flex items-start gap-4">
                      <div className="h-10 w-10 rounded-full bg-primary/10 flex items-center justify-center flex-shrink-0">
                        <Phone className="h-5 w-5 text-primary" />
                      </div>
                      <div>
                        <h4 className="font-semibold mb-1">Phone</h4>
                        <p className="text-muted-foreground">+91 9927153555</p>
                        <p className="text-sm text-muted-foreground">Mon-Sat, 9 AM - 6 PM</p>
                      </div>
                    </div>

                    <div className="flex items-start gap-4">
                      <div className="h-10 w-10 rounded-full bg-primary/10 flex items-center justify-center flex-shrink-0">
                        <Mail className="h-5 w-5 text-primary" />
                      </div>
                      <div>
                        <h4 className="font-semibold mb-1">Email</h4>
                        <p className="text-muted-foreground">bulkorders@sunfead.com</p>
                        <p className="text-sm text-muted-foreground">We respond within 24 hours</p>
                      </div>
                    </div>
                  </div>
                </div>

                <Card className="bg-primary/5 border-primary/20 p-6">
                  <h4 className="font-semibold mb-3">Minimum Order Quantities</h4>
                  <div className="space-y-2 text-sm text-muted-foreground">
                    <p>• Namkeen & Snacks: 50 kg minimum</p>
                    <p>• Pasta & Vermicelli: 100 kg minimum</p>
                    <p>• Poha: 75 kg minimum</p>
                    <p>• Mixed Products: Contact for details</p>
                  </div>
                </Card>

                <Card className="bg-secondary/5 border-secondary/20 p-6">
                  <h4 className="font-semibold mb-3">Payment Terms</h4>
                  <div className="space-y-2 text-sm text-muted-foreground">
                    <p>• Flexible payment options available</p>
                    <p>• Credit facilities for regular customers</p>
                    <p>• Advance payment discounts</p>
                  </div>
                </Card>
              </div>

              {/* Quote Form */}
              <Card className="p-6 md:p-8">
                <form onSubmit={handleSubmit} className="space-y-6">
                  <div>
                    <label htmlFor="name" className="block text-sm font-medium mb-2">
                      Full Name *
                    </label>
                    <Input
                      id="name"
                      name="name"
                      type="text"
                      required
                      placeholder="Your name"
                      value={formData.name}
                      onChange={handleChange}
                    />
                  </div>

                  <div>
                    <label htmlFor="company" className="block text-sm font-medium mb-2">
                      Company Name
                    </label>
                    <Input
                      id="company"
                      name="company"
                      type="text"
                      placeholder="Your company name"
                      value={formData.company}
                      onChange={handleChange}
                    />
                  </div>

                  <div>
                    <label htmlFor="email" className="block text-sm font-medium mb-2">
                      Email Address *
                    </label>
                    <Input
                      id="email"
                      name="email"
                      type="email"
                      required
                      placeholder="your.email@example.com"
                      value={formData.email}
                      onChange={handleChange}
                    />
                  </div>

                  <div>
                    <label htmlFor="phone" className="block text-sm font-medium mb-2">
                      Phone Number *
                    </label>
                    <Input
                      id="phone"
                      name="phone"
                      type="tel"
                      required
                      placeholder="+91 98765 43210"
                      value={formData.phone}
                      onChange={handleChange}
                    />
                  </div>

                  <div>
                    <label htmlFor="productInterest" className="block text-sm font-medium mb-2">
                      Product Interest *
                    </label>
                    <Input
                      id="productInterest"
                      name="productInterest"
                      type="text"
                      required
                      placeholder="e.g., Pasta, Namkeen, Poha"
                      value={formData.productInterest}
                      onChange={handleChange}
                    />
                  </div>

                  <div>
                    <label htmlFor="quantity" className="block text-sm font-medium mb-2">
                      Estimated Quantity *
                    </label>
                    <Input
                      id="quantity"
                      name="quantity"
                      type="text"
                      required
                      placeholder="e.g., 100 kg, 500 packets"
                      value={formData.quantity}
                      onChange={handleChange}
                    />
                  </div>

                  <div>
                    <label htmlFor="message" className="block text-sm font-medium mb-2">
                      Additional Details
                    </label>
                    <Textarea
                      id="message"
                      name="message"
                      placeholder="Tell us about your requirements, delivery timeline, etc."
                      rows={4}
                      value={formData.message}
                      onChange={handleChange}
                    />
                  </div>

                  <Button type="submit" className="w-full festival-gradient border-0 text-white">
                    Request Quote
                  </Button>
                </form>
              </Card>
            </div>
          </div>
        </div>
      </section>

      <Footer />
    </div>
  );
};

export default BulkOrder;
