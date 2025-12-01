import Navbar from "@/components/Navbar";
import Footer from "@/components/Footer";
import { Button } from "@/components/ui/button";
import { Link } from "react-router-dom";
import { Clock, ArrowLeft } from "lucide-react";

const ComingSoon = () => {
  return (
    <div className="min-h-screen bg-background flex flex-col">
      <Navbar />
      
      <div className="flex-1 flex items-center justify-center py-16 px-4">
        <div className="max-w-2xl mx-auto text-center">
          <div className="mb-8 flex justify-center">
            <div className="h-24 w-24 rounded-full bg-primary/10 flex items-center justify-center">
              <Clock className="h-12 w-12 text-primary" />
            </div>
          </div>
          
          <h1 className="text-4xl md:text-5xl lg:text-6xl font-display font-bold mb-6">
            Coming <span className="text-gradient">Soon</span>
          </h1>
          
          <p className="text-lg md:text-xl text-muted-foreground mb-8 max-w-xl mx-auto">
            We're working hard to bring you this feature. Stay tuned for exciting updates!
          </p>

          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            <Link to="/">
              <Button size="lg" className="festival-gradient border-0 text-white">
                <ArrowLeft className="mr-2 h-5 w-5" />
                Back to Home
              </Button>
            </Link>
            <Link to="/shop">
              <Button size="lg" variant="outline" className="border-2">
                Browse Products
              </Button>
            </Link>
          </div>

          <div className="mt-16 pt-16 border-t">
            <p className="text-sm text-muted-foreground mb-4">
              Want to be notified when this feature launches?
            </p>
            <p className="text-sm text-muted-foreground">
              Follow us on social media or contact us at{" "}
              <a href="mailto:info@sunfead.com" className="text-primary hover:underline">
                info@sunfead.com
              </a>
            </p>
          </div>
        </div>
      </div>

      <Footer />
    </div>
  );
};

export default ComingSoon;
